
$inputFile=$args[0]
$outputFile=$args[1]

# ----------------------------------------
# Read all necessary JSON files
# ----------------------------------------

$calendarJson = (gc "$PSScriptRoot\calendar.json" -encoding utf8) | ConvertFrom-Json

$web_client = new-object system.net.webclient
$colorsJson = $web_client.DownloadString("https://raw.githubusercontent.com/ozh/github-colors/master/colors.json") | ConvertFrom-Json

# ----------------------------------------
# Create SVG tiles
# ----------------------------------------

$tilesFolder = "$PSScriptRoot\Tiles"
$readmeInput = ""

foreach ($codeSet in $calendarJson.CodeSets) {
    $year = $codeSet.Year
    $language = $codeSet.Language
    $directory = $codeSet.Directory
    
    $tilesForYear = "$tilesFolder\$year"
    New-Item "$tilesForYear" -itemType Directory
    
    Get-ChildItem "$PSScriptRoot\..\$directory" -Directory -Filter * | Foreach-Object {
        $day = ""
        $folderName = $_.Name
        if ([int]::TryParse($folderName,[ref]$day))
        {
            $dayWithZero = '{0:d2}'-f $day
            $svg = Get-Content "$PSScriptRoot\calendar-tile.svg"

            $svg = $svg -replace 'XXX', $dayWithZero
            $svg = $svg -replace 'PART_1_TIME', ''
            $svg = $svg -replace 'PART_2_TIME', ''
            $svg = $svg -replace 'LANGUAGE', $language
            $svg = $svg -replace '00FF00', '178600'

            $svg | Out-File -encoding utf8 ("$tilesForYear\$dayWithZero.svg")

            $readmeInput = $readmeInput + "<a href=`"./$directory/$folderName`"><img src=`"./Calendar/Tiles/$year/$dayWithZero.svg`" width=`"150px`"></a>`n"
        }
    }
}

# ----------------------------------------
# Read all necessary JSON files
# ----------------------------------------

$readme = Get-Content $inputFile

$readme = $readme -replace '<!-- CALENDAR-TILES -->', $readmeInput

$readme | Out-File -encoding utf8 $outputFile