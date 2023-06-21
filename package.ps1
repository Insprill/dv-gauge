param (
    [switch]$NoArchive,
    [string]$OutputDirectory = $PSScriptRoot
)

Set-Location "$PSScriptRoot"

$DistDir = "$OutputDirectory/dist"
if ($NoArchive)
{
    $ZipWorkDir = "$OutputDirectory"
}
else
{
    $ZipWorkDir = "$DistDir/tmp"
}
$ZipRootDir = "$ZipWorkDir/BepInEx"
$ZipInnerDir = "$ZipRootDir/plugins/Gauge/"
$BuildDir = "build"
$LicenseFile = "LICENSE"
$GaugeDll = "$BuildDir/Gauge.dll"
$AssetBundle = "$BuildDir/gauge.assetbundle"

New-Item "$ZipInnerDir" -ItemType Directory -Force
Copy-Item -Force -Path "$LicenseFile", "$GaugeDll", "$AssetBundle" -Destination "$ZipInnerDir"

if (!$NoArchive)
{
    $VERSION = (Select-String -Pattern '([0-9]+\.[0-9]+\.[0-9]+)' -Path Gauge/Gauge.cs).Matches.Value
    $FILE_NAME = "$DistDir/Gauge_v$VERSION.zip"
    Compress-Archive -Update -CompressionLevel Fastest -Path "$ZipRootDir" -DestinationPath "$FILE_NAME"
}
