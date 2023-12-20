$targetFolder = $args[0]
$year = $args[1]
$day = $args[2]
$language = $args[3]

$dayWithZero = '{0:d2}' -f [int]$day

# ----------------------------------------
# Helper functions
# ----------------------------------------

function ReplaceToken($content)
{
    $content = $content -replace '{ClassName}', $className
    $content = $content -replace '{year}', $year
    $content = $content -replace '{day}', $day
    $content = $content -replace '{dayWithZero}', $dayWithZero
    $content = $content -replace '{Title}', $title

    return $content
}

# ----------------------------------------
# Read all necessary JSON files
# ----------------------------------------

$environmentJson = (gc "$PSScriptRoot\environment.json" -encoding utf8) | ConvertFrom-Json
if (!$environmentJson.Cookie)
{
    throw "Environment file with cookie was not found!"
}

$webClient = new-object system.net.webclient
$webClient.Headers.add("Cookie", -join("session=", $environmentJson.Cookie))

# ----------------------------------------
# Fetch the title of the day
# ----------------------------------------

$dayContent = $webClient.DownloadString("https://adventofcode.com/$year/day/$day")
# $dayContent = [string] (gc "$PSScriptRoot\Examples\puzzle-2023.html" -encoding utf8)

$regex = '<h2>--- Day ' + $day + ': ([^-]*) ---<\/h2>'
$title = 'TODO!!!'
foreach ($titleGroup in ([regex]$regex).Matches($dayContent)) {
    $title = $titleGroup.Groups.Groups[1].Value.Trim()
}

$First = $title -Replace '[^0-9A-Z]', ' ' -Split ' '
$className = (Get-Culture).TextInfo.ToTitleCase($First) -Replace ' '
# Write-Output ("className: " + ($className))


# ----------------------------------------
# Copy all the files from the template dir
# ----------------------------------------

$templateDirectory = "$PSScriptRoot\Templates\$language"
$targetDirectory = "$targetFolder\$year\$dayWithZero"

if (Test-Path -Path $templateDirectory)
{
    if (!(Test-Path -Path $targetDirectory)) {
        New-Item $targetDirectory -itemType Directory
    }
    
    foreach ($templateFile in (Get-ChildItem $templateDirectory))
    {
        $templateFileName = $templateDirectory + "\" + $templateFile.Name;

        $content = Get-Content ($templateFileName)
        $content = ReplaceToken($content)

        $targetFileName = $targetDirectory + "\" + (ReplaceToken($templateFile.Name))
        $content | Out-File -encoding utf8 $targetFileName
    }
}