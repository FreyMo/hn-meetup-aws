---
# try also 'default' to start simple
theme: default
# random image from a curated Unsplash collection by Anthony
# like them? see https://unsplash.com/collections/94734566/slidev
background: https://cover.sli.dev
# some information about your slides, markdown enabled
title: AWS User Group Heilbronn

# apply any unocss classes to the current slide
class: text-center
# https://sli.dev/custom/highlighters.html
highlighter: shiki
# https://sli.dev/guide/drawing
drawings:
  persist: false
# slide transition: https://sli.dev/guide/animations#slide-transitions
transition: slide-left
# enable MDC Syntax: https://sli.dev/guide/syntax#mdc-syntax
mdc: true
---

# C# in AWS

Using C# to develop applications and cloud infrastructure

https://github.com/FreyMo/hn-meetup-aws

---
transition: fade-out
layout: image-right
image: https://avatars.githubusercontent.com/u/25849260?v=4
backgroundSize: 60%
---

# About me

## Moritz Freyburger

Freelancer & Cloud Developer

* 🤓 **Multilingual**
  * C#/F#/.NET
  * Rust
  * TypeScript/JavaScript
  * C++, Go, ...
* ☁️ **Cloud Developer for many years**
  * Azure (Terraform, Bicep, ARM)
  * AWS (Terraform, CDK)
* 🛠 **DevOps Engineer for even longer**
  * Docker, CI/CD, IaC, Scripting

<div class="abs-br m-6 flex gap-2">
  <a href="https://github.com/FreyMo" target="_blank" alt="GitHub" title="Open in GitHub"
    class="text-xl slidev-icon-btn opacity-50 !border-none !hover:text-white">
    <carbon-logo-github />
  </a>
  <a href="https://www.linkedin.com/in/moritz-freyburger-9845ab275" target="_blank" alt="LinkedIn" title="Open in LinkedIn"
    class="text-xl slidev-icon-btn opacity-50 !border-none !hover:text-white">
    <carbon-logo-linkedin />
  </a>
  <a href="https://freyburger.io" target="_blank" alt="Website" title="Open in Browser"
    class="text-xl slidev-icon-btn opacity-50 !border-none !hover:text-white">
    <carbon-application-web />
  </a>
</div>

---
transition: slide-up
level: 2
layout: image-right
image: https://cover.sli.dev
---

# Introduction to C#

* "Mature" language from 2000
* Latest Release C# 12
* GC, Compiled (JIT and AOT), multi-purpose
* mostly imperative and OO, functional
* Free, open source and cross platform*
* Runs on the .NET runtime*, also:
  * F#, PowerShell, Visual Basic

<!-- <br> -->

Hello World:

````md magic-move {lines: true}
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Hello, World!");
    }
}
```

```csharp
Console.WriteLine("Hello, World!");
```
````

---
transition: slide-up
level: 2
layout: two-cols-header
---

# Comparing .NET Runtimes

<img src="https://dotnet.microsoft.com/blob-assets/images/illustrations/release-schedule-dark.svg" alt="Description" style="display: block; margin-left: auto; margin-right: auto; width:60%; height:auto;">

::left::

## .NET Framework

* old & legacy
* partially closed source
* Windows only
* Latest version: 4.8.1
* LTS only, no further development

::right::

## .NET (Core)

* new & shiny
* fully open source
* cross-platform
* Latest version: 8.0
* very active community & development
* Better tooling, performance etc.

---
transition: slide-up
layout: two-cols-header
level: 2
---

# The elefant in the room

Why would you use Microsoft technology in AWS and not Azure?

::left::

<v-click>

- Satya Nadella superseded Steve Ballmer
- Microsofts OSS philosophy
  - acquired GitHub, npm
  - developed VS Code
  - .NET Core fully open source
- push to use Linux (WSL 2 anyone?)
- Changing business model (Azure & 365 SaaS)
- Cloud providers are fishing for customers

</v-click>

::right::

<img src="https://news.microsoft.com/wp-content/uploads/prod/2014/09/MS-Exec-Nadella-Satya-2017-08-31-14-683x1024.jpg" alt="Description" style="margin-left: 25%; width:60%; height:auto;">

---
transition: slide-up
level: 4
layout: center
---

# The Good

- High development speed
- Fast enough for most cloud use cases
- AWS has great support for C# (and even F#)
  - Backend (Lambda, WebAPIs, batch jobs, CLIs)
  - Frontend (MVC, Razor Pages, Blazor)
- IMO: Nice syntax
- Indisputable: **great** ecosystem
  - `async/await` & TPL
  - ASP.NET Core [is commonly used](https://survey.stackoverflow.co/2023/#section-most-popular-technologies-web-frameworks-and-technologies) and [quite popular](https://survey.stackoverflow.co/2023/#section-admired-and-desired-web-frameworks-and-technologies)
    - Auth, Swagger, Middlewares, JSON, etc.
  - NuGet & tooling (`dotnet` CLI), Docker images
  - Timers, Encryption, Logging, DI, Service lifetimes, Entity Framework
- OSS maintained by a big company
- [modern .NET is heavily used](https://survey.stackoverflow.co/2023/#section-most-popular-technologies-other-frameworks-and-libraries)

---
transition: slide-up
level: 2
layout: center
---

# The Bad

- GC, so performance may become a problem
- IMO: exception-based error handling
  - Similar languages: JS/TS, Java, C++, ...
  - Better examples: Rust, Go, F#, ...
- Some do not like the OOP approach
- Some do not like strongly typed languages
- Some do not like Microsoft
- Bad reputation because of .NET Framework

---
transition: slide-up
level: 2
layout: center
---

# The Ugly

\-

---
transition: slide-up
layout: center
class: text-center
---

# Demo

Creating a PDF report generator

![diagram](/diagram.png)

---
level: 2
layout: two-cols-header
image: https://cover.sli.dev
---

# Wrapping it up

Should you use C# in AWS?

* First class support for C# as a language in AWS  
* You can develop both infrastructure and applications
  * CDK and SDK
  * Lambda runtime, containers etc.

::left::
Yes, if
* you are already using C# at work
* you want a nice high level language with
  * good performance
  * great tooling and
  * a large community

::right::

Not necessarily, if
* you are already fluent in a similar language like TS/JS, Java, etc.
* you are squeezing out the last bit of performance
* you have a very specific use case (e.g., Pythons ML libraries)
* you despise the Microsoft ecosystem

---
layout: center
layout: image-right
image: https://cover.sli.dev
---

# Thanks

Any questions?

https://github.com/FreyMo/hn-meetup-aws
