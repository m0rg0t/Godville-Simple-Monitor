# Portable source regressions

## Fix
Read the current XML element before advancing: ReadElementContentAsString already advances, so ReadToFollowing skipped adjacent inventory entries. Dispose the reader reliably. Use health from the response instead of character zero (numeric 48), and parse XML health values with invariant culture.

## Run
With .NET SDK 10.0.401 installed:

    dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release

This is a dependency-free net10.0 console harness with C# 5 syntax checking. Its project links the actual production files, without copying their logic. NuGet sources are cleared. The SDK runs the tests only; the historical application is not retargeted.

## Coverage
Actual MainPage parser and response handler against synthetic XML: adjacent/separated entries, absent elements, malformed input fallback and fractional health under en-US/ru-RU cultures.

## Original failure
The same tests were run against original source at [707428e3da2ee8a4ffd5820e5a334207aeefa8d2](https://github.com/m0rg0t/Godville-Simple-Monitor/tree/707428e3da2ee8a4ffd5820e5a334207aeefa8d2) and exited nonzero; the repaired source exits zero.

    FAIL: adjacent inventory elements: Assertion failed
    FAIL: health reflects response in en-US: Assertion failed
    FAIL: health reflects response in ru-RU: The input string '100.5

## Boundaries
The stand-ins in Portable.Tests/Stubs.cs only supply unavailable platform types; they do not prove platform compatibility or simulate the app UI. No real service/user data, credentials, notifications, payments, publishing or deployment are used.

Phone dispatcher/threading, live requests, timer lifecycle, ads and XAML/device behavior are unverified. Network and message-box stand-ins throw if used.

The full historical Windows/Phone build and device runtime remain unverified. Original project targets, package manifests, vendor binaries and assets are unchanged. No compatible modern replacement for the old platform-specific dependencies has been established, so no speculative package upgrade is included.
