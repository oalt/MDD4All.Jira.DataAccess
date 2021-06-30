# MDD4All.Jira.DataAccess

[![Nuget Package Build](https://github.com/oalt/MDD4All.Jira.DataAccess/actions/workflows/build.yml/badge.svg)](https://github.com/oalt/MDD4All.Jira.DataAccess/actions/workflows/build.yml)

REST Data Access implementation for Jira. 

The Atlassian JiraREST API is documented here: https://developer.atlassian.com/cloud/jira/software/rest/intro/

# Usage

## Preconditions
* Jira Cloud access
* You have to generate an API key for your Jira account

## Basic steps
* Add the nuget package to your project
* Add the using directive to use the ``JiraRestClient``
```C#
using MDD4All.Jira.DataAccess

...
```

## Initialization

You have to provide the serverURL (cloud URL), the user name (typically an E-Mail address) and your Jira API key.

```C#
private JiraRestClient _jiraClient;

...

// initialize the Jira Rest Client with server URL, username and apiKey
_jiraClient = new JiraRestClient(url, userName, apiKey);

``` 

## Get issue by key

Get issue data object for API version 3.

* The issueKey is the Jira issue key as a string similar to ``TEST-45``.
 
```C#
using Jira3 = MDD4All.Jira.DataModels.V3;
...

Jira3.Issue issue = await _jiraClient.GetJiraIssueAsync(issueKey);
```

## Get issue list by JQL query

Get an issue list using a JQL expression (e.g. ``jql = "project = \"" + projectID + "\" and type in (Requirement, \"Customer Requirement\")";``

```C#
IssueSearchResponse result = await _jiraClient.GetIssuesByJQL(jql);
```

The result will contain a list of ``Issue`` objects.

## Get a list of all projects

```C#
ProjectSearchResponse projectSearchResponse = await _jiraClient.GetJiraProjectsAsync();
```

## Get a specific project

```C#
Project project = await _jiraClient.GetJiraProjectByKeyAsync(projectKey);
```

## Create a new issue

* Create a new issue using the API version 2.
* issueData is an ``MDD4All.Jira.DataModels.Issue`` data object. 

```C#
Issue newIssue = await _jiraClient.CreateJiraIssueAsync(issueData);
```
