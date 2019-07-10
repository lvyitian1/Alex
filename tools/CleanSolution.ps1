param (
    [string[]]$Filter = @('bin', 'obj', '.vs'),
    [switch]$DryRun = $false,
    [string]$SolutionName = $((Get-Item (Split-Path $script:MyInvocation.MyCommand.Path)).Parent.Name),
    [string]$OutputDir = "output",
    [string]$SrcDir = "src",
    [switch]$Force = $false
)


Write-Host "Cleaning Solution ""$SolutionName"" with filter ""$Filter""";

$ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path;
$BasePath = (Get-Item $ScriptDir).Parent.FullName;

$OutputPath = "$BasePath\$OutputDir\";
$SrcPath = "$BasePath\$SrcDir";
$VsCachePath = "$SrcPath\.vs";

$SolutionPath = "$SrcPath\$SolutionName.sln";

if (Test-Path $OutputPath)
{
  Write-Host "Clearing Output";

  Get-ChildItem -LiteralPath $OutputPath -Recurse | foreach($_) { Write-Host "Removing "($_.FullName); if(!$DryRun) { Remove-Item $_.FullName -Recurse -Force:$Force -WhatIf:$DryRun;  }};

  Write-Host "Output Cleaned";
}
else
{
    Write-Warning "Output Directory ""$OutputPath"" not found";
}

if ((Test-Path $SolutionPath) -and (Test-Path $SrcPath))
{
    Write-Host "Cleaning Solution";

	$SolutionDir = (Get-Item $SolutionPath).Parent.FullName;
	
    Write-Host "Removing all folders matching ($Filter) in $SrcPath";

    Push-Location $SrcPath

        Get-ChildItem -Include $Filter -Recurse | foreach($_) { Write-Host "Removing bin,obj "($_.FullName); if(!$DryRun) { Remove-Item $_.FullName -Recurse -Force:$Force -WhatIf:$DryRun;  }}
    
    Pop-Location

    Write-Host "Solution Cleaned."
}
else
{
    Write-Warning "Solution Directory ""$SolutionPath"" not found";
}

Write-Host "Clean Completed."