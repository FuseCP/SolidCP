cd .\Debian
$size = ((ls -r|measure -s Length).Sum / 1024).ToString("f0")
$version = $args[0]
$control = Get-Content .\DEBIAN\control -Raw
$control = [Regex]::Replace($control, "^Installed-Size:.*$`n?","", [Text.RegularExpressions.RegexOptions]::Multiline)
$control = [Regex]::Replace($control, "(?<=^Version:\s*).*$", $version, [Text.RegularExpressions.RegexOptions]::Multiline)
$control = [String]::Concat($control, "Installed-Size: $size`n")
[Environment]::CurrentDirectory = (Get-Location).Path
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[IO.File]::WriteAllText(".\DEBIAN\control", $control, $Utf8NoBomEncoding)
cd ..