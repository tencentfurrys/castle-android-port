# repack-p12t.ps1 - package the p12t Android test build:
#   raw Unity APK -> apktool decode -> (targetSdk 28, versionCode 5/1.1.2t, splash off)
#   -> apktool build -> zipalign -> jarsigner (castle-debug) -> re-zipalign -> verify
param(
  [string]$RawApk  = "D:\castle-port-56\unity-project\Castle-1.1.2-android.apk",
  [string]$WorkRoot = "D:\castle-port-56\pkg",
  [string]$Java     = "D:\Unity-5.3\jdk8\bin\java.exe",
  [string]$Jarsigner= "D:\Unity-5.3\jdk8\bin\jarsigner.exe",
  [string]$Zipalign = "D:\Unity-5.3\sdk\build-tools\23.0.3\zipalign.exe",
  [string]$Aapt     = "D:\Unity-5.3\sdk\build-tools\23.0.3\aapt.exe",
  [string]$Apktool  = "D:\castle-tools\apktool_2.9.3.jar",
  [string]$Keystore = "D:\castle-port\castle-debug.keystore",
  [string]$OutApk   = "D:\castle-port-56\pkg\Castle-1.1.2-android-p12t-t28.apk"
)
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.IO.Compression.FileSystem
function FindBytes([byte[]]$hay, [byte[]]$needle) {
  $n = $needle.Length; $hn = $hay.Length
  for ($i = 0; $i -le $hn - $n; $i++) {
    $ok = $true
    for ($j = 0; $j -lt $n; $j++) { if ($hay[$i + $j] -ne $needle[$j]) { $ok = $false; break } }
    if ($ok) { return $i }
  }
  return -1
}
if (-not (Test-Path $RawApk)) { throw "raw apk missing: $RawApk" }
$decoded  = Join-Path $WorkRoot "decoded-v2"
$unsigned = Join-Path $WorkRoot "Castle-p12t-unsigned.apk"
$aligned  = Join-Path $WorkRoot "Castle-p12t-aligned.apk"
$signed   = Join-Path $WorkRoot "Castle-p12t-aligned-signed.apk"
if (Test-Path $decoded) { Remove-Item $decoded -Recurse -Force }

Write-Output "=== [1/6] apktool decode ==="
& $Java -jar $Apktool d -f -o $decoded $RawApk
if ($LASTEXITCODE -ne 0) { throw "apktool decode failed: $LASTEXITCODE" }

Write-Output "=== [2/6] edit apktool.yml + settings.xml ==="
$yml = Join-Path $decoded "apktool.yml"
$y = [System.IO.File]::ReadAllText($yml)
$y = [regex]::Replace($y, "targetSdkVersion: '\d+'", "targetSdkVersion: '28'")
$y = [regex]::Replace($y, "versionCode: \d+", "versionCode: 5")
$y = [regex]::Replace($y, "versionName: [^\r\n]+", "versionName: 1.1.2t")
[System.IO.File]::WriteAllText($yml, $y)
Write-Output "--- apktool.yml key lines ---"
Select-String -Path $yml -Pattern "minSdkVersion|targetSdkVersion|versionCode|versionName" | ForEach-Object { $_.Line.Trim() }
$sx = Join-Path $decoded "assets\bin\Data\settings.xml"
$s = [System.IO.File]::ReadAllText($sx)
$s = $s.Replace('<bool name="showSplash">True</bool>', '<bool name="showSplash">False</bool>')
[System.IO.File]::WriteAllText($sx, $s)
Write-Output "--- settings.xml after edit ---"
Write-Output ([System.IO.File]::ReadAllText($sx))

Write-Output "=== [3/6] apktool build ==="
& $Java -jar $Apktool b $decoded -o $unsigned
if ($LASTEXITCODE -ne 0) { throw "apktool build failed: $LASTEXITCODE" }

Write-Output "=== [4/6] zipalign + jarsigner ==="
if (Test-Path $aligned) { Remove-Item $aligned -Force }
& $Zipalign -f 4 $unsigned $aligned
if ($LASTEXITCODE -ne 0) { throw "zipalign (pre-sign) failed: $LASTEXITCODE" }
if (Test-Path $signed) { Remove-Item $signed -Force }
Copy-Item $aligned $signed -Force
& $Jarsigner -keystore $Keystore -storepass android -keypass android -sigalg SHA1withRSA -digestalg SHA1 $signed castle
if ($LASTEXITCODE -ne 0) { throw "jarsigner failed: $LASTEXITCODE" }

Write-Output "=== [5/6] re-zipalign signed -> final ==="
if (Test-Path $OutApk) { Remove-Item $OutApk -Force }
& $Zipalign -f 4 $signed $OutApk
if ($LASTEXITCODE -ne 0) { throw "zipalign (final) failed: $LASTEXITCODE" }

Write-Output "=== [6/6] verification ==="
Write-Output "--- jarsigner verify (final apk) ---"
$va = & $Jarsigner -verify -verbose -certs $OutApk 2>&1 | Select-String "jar verified|jar is unsigned|not verified"
$va | ForEach-Object { $_.Line.Trim() }
Write-Output "--- cert fingerprint ---"
& $Jarsigner -verify -verbose -certs $OutApk 2>&1 | Select-String "SHA256:" | Select-Object -First 2 | ForEach-Object { $_.Line.Trim() }
Write-Output "--- zipalign check (final apk) ---"
& $Zipalign -c -v 4 $OutApk 2>&1 | Select-Object -Last 2 | ForEach-Object { $_ }
Write-Output "--- badging ---"
if (Test-Path $Aapt) { & $Aapt dump badging $OutApk 2>$null | Select-String "package:|sdkVersion|targetSdkVersion|native-code" | Select-Object -First 8 | ForEach-Object { $_.Line.Trim() } } else { Write-Output "aapt not found - skipped" }
Write-Output "--- tracer markers in Assembly-CSharp.dll ---"
$zip = [System.IO.Compression.ZipFile]::OpenRead($OutApk)
$e = $zip.Entries | Where-Object { $_.FullName -eq "assets/bin/Data/Managed/Assembly-CSharp.dll" }
$ms = New-Object System.IO.MemoryStream; $st = $e.Open(); $st.CopyTo($ms); $st.Dispose(); $zip.Dispose(); $b = $ms.ToArray()
$t1 = FindBytes $b ([System.Text.Encoding]::Unicode.GetBytes("tracer["))
$t2 = FindBytes $b ([System.Text.Encoding]::Unicode.GetBytes("p12t"))
$t3 = FindBytes $b ([System.Text.Encoding]::ASCII.GetBytes("CastleBootTracer"))
Write-Output ("dll bytes: " + $b.Length + " ; 'tracer[' utf16 at: " + $t1 + " ; 'p12t' utf16 at: " + $t2 + " ; 'CastleBootTracer' ascii at: " + $t3)
Write-Output "--- final file ---"
Get-Item $OutApk | ForEach-Object { $_.FullName + "  " + $_.Length + "  " + $_.LastWriteTime }
Write-Output "ALL DONE"
