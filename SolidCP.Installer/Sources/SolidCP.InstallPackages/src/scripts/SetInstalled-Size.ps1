cd .\Debian
$size = ((ls -r|measure -s Length).Sum / 1024).ToString("f0")
$control = Get-Content .\DEBIAN\control -Raw
$control = [Regex]::Replace($control, "^Installed-Size:.*$`n","", [Text.RegularExpressions.RegexOptions]::Multiline)
$control = [String]::Concat($control, "Installed-Size: $size")
Set-Content -Encoding utf8 -Path .\DEBIAN\control -Value $control
cd ..