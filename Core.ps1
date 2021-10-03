Import-Module PowershellForXti -Force

$script:coreConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "XTI_Core"
    AppName = "XTI_Core"
    AppType = "Package"
}

function Core-NewVersion {
    param(
        [Parameter(Position=0)]
        [ValidateSet("major", "minor", "patch")]
        $VersionType
    )
    $script:coreConfig | New-XtiVersion @PsBoundParameters
}

function Core-NewIssue {
    param(
        [Parameter(Mandatory, Position=0)]
        [string] $IssueTitle,
        [switch] $Start
    )
    $script:coreConfig | New-XtiIssue @PsBoundParameters
}

function Core-StartIssue {
    param(
        [Parameter(Position=0)]
        [long]$IssueNumber = 0
    )
    $script:coreConfig | Xti-StartIssue @PsBoundParameters
}

function Core-CompleteIssue {
    param(
    )
    $script:coreConfig | Xti-CompleteIssue @PsBoundParameters
}

function Core-Publish {
    param(
        [ValidateSet("Development", "Production", "Staging", "Test")]
        $EnvName = "Development"
    )
    $script:coreConfig | Xti-Publish @PsBoundParameters
}
