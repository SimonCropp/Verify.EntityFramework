<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> Verify.EntityFramework

[![Build status](https://ci.appveyor.com/api/projects/status/g6njwv0aox62atu0?svg=true)](https://ci.appveyor.com/project/SimonCropp/verify-entityframework)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.EntityFramework.svg)](https://www.nuget.org/packages/Verify.EntityFramework/)

Extends [Verify](https://github.com/SimonCropp/Verify) to allow verification of EntityFramewok bits.


<!-- toc -->
## Contents

  * [Usage](#usage)
    * [Added entry](#added-entry)
    * [Deleted entry](#deleted-entry)
    * [Modified entry](#modified-entry)<!-- endtoc -->


## NuGet package

https://nuget.org/packages/Verify.EntityFramework/


## Usage

Enable VerifyEntityFramewok once at assembly load time:

<!-- snippet: Enable -->
<a id='snippet-enable'/></a>
```cs
VerifyEntityFramework.Enable();
```
<sup><a href='/src/Tests/GlobalSetup.cs#L9-L11' title='File snippet `enable` was extracted from'>snippet source</a> | <a href='#snippet-enable' title='Navigate to start of snippet `enable`'>anchor</a></sup>
<!-- endsnippet -->


### Added entry

This test:

<!-- snippet: Added -->
<a id='snippet-added'/></a>
```cs
[Fact]
public async Task Added()
{
    var options = DbContextOptions();

    await using var context = new SampleDbContext(options);
    context.Companies.Add(new Company {Content = "before"});
    await Verify(context);
}
```
<sup><a href='/src/Tests/Tests.cs#L12-L22' title='File snippet `added` was extracted from'>snippet source</a> | <a href='#snippet-added' title='Navigate to start of snippet `added`'>anchor</a></sup>
<!-- endsnippet -->

Will result in the following verified file:

<!-- snippet: Tests.Added.verified.txt -->
<a id='snippet-Tests.Added.verified.txt'/></a>
```txt
{
  Added: {
    Company: {
      Id: 0,
      Content: 'before'
    }
  }
}
```
<sup><a href='/src/Tests/Tests.Added.verified.txt#L1-L8' title='File snippet `Tests.Added.verified.txt` was extracted from'>snippet source</a> | <a href='#snippet-Tests.Added.verified.txt' title='Navigate to start of snippet `Tests.Added.verified.txt`'>anchor</a></sup>
<!-- endsnippet -->


### Deleted entry

This test:

<!-- snippet: Deleted -->
<a id='snippet-deleted'/></a>
```cs
[Fact]
public async Task Deleted()
{
    var options = DbContextOptions();

    await using (var context = new SampleDbContext(options))
    {
        context.Companies.Add(new Company {Content = "before"});
        context.SaveChanges();
    }

    await using (var context = new SampleDbContext(options))
    {
        var company = context.Companies.Single();
        context.Companies.Remove(company);
        await Verify(context);
    }
}
```
<sup><a href='/src/Tests/Tests.cs#L24-L43' title='File snippet `deleted` was extracted from'>snippet source</a> | <a href='#snippet-deleted' title='Navigate to start of snippet `deleted`'>anchor</a></sup>
<!-- endsnippet -->

Will result in the following verified file:

<!-- snippet: Tests.Deleted.verified.txt -->
<a id='snippet-Tests.Deleted.verified.txt'/></a>
```txt
{
  Deleted: {
    Company: {
      Id: 0
    }
  }
}
```
<sup><a href='/src/Tests/Tests.Deleted.verified.txt#L1-L7' title='File snippet `Tests.Deleted.verified.txt` was extracted from'>snippet source</a> | <a href='#snippet-Tests.Deleted.verified.txt' title='Navigate to start of snippet `Tests.Deleted.verified.txt`'>anchor</a></sup>
<!-- endsnippet -->


### Modified entry

This test:

<!-- snippet: Modified -->
<a id='snippet-modified'/></a>
```cs
[Fact]
public async Task Modified()
{
    var options = DbContextOptions();

    await using (var context = new SampleDbContext(options))
    {
        context.Companies.Add(new Company {Content = "before"});
        context.SaveChanges();
    }

    await using (var context = new SampleDbContext(options))
    {
        context.Companies.Single().Content = "after";
        await Verify(context);
    }
}
```
<sup><a href='/src/Tests/Tests.cs#L45-L63' title='File snippet `modified` was extracted from'>snippet source</a> | <a href='#snippet-modified' title='Navigate to start of snippet `modified`'>anchor</a></sup>
<!-- endsnippet -->

Will result in the following verified file:

<!-- snippet: Tests.Modified.verified.txt -->
<a id='snippet-Tests.Modified.verified.txt'/></a>
```txt
{
  Modified: {
    Company: {
      Id: 0,
      Content: {
        Original: 'before',
        Current: 'after'
      }
    }
  }
}
```
<sup><a href='/src/Tests/Tests.Modified.verified.txt#L1-L11' title='File snippet `Tests.Modified.verified.txt` was extracted from'>snippet source</a> | <a href='#snippet-Tests.Modified.verified.txt' title='Navigate to start of snippet `Tests.Modified.verified.txt`'>anchor</a></sup>
<!-- endsnippet -->


## Icon

[Spider](https://thenounproject.com/term/spider/904683/) designed by [marialuisa iborra](https://thenounproject.com/marialuisa.iborra/) from [The Noun Project](https://thenounproject.com/creativepriyanka).
