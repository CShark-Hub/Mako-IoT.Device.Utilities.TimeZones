# Mako-IoT.Device.Utilities.TimeZones
The way to get local date & time on your device. Given time zone definition the library converts UTC to local time, handles Daylight Saving Time transitions as well.

## Usage
### Create _TimeZone_ object
Time zone without DST
```c#
var timezone = new TimeZone(utcOffset);
```
Time zone with DST
```c#
var timezone = new TimeZone(standardUtcOffset, dstUtcOffset, dstStarts, dstEnds);
```
### DST transition dates
Time zones across the globe have various rules about transition to and from DST. To accomodate that we can provide transition dates in two ways:
1. **_FixedDate_** - fixed month and day of the year
```c#
//DST starts every year on the 20th of June at 2:00 AM
var dstStarts = new FixedDate(new DateTime(2023, 20, 6, 2, 0, 0));
```
2. **_FloatingDate_** - weekday of given week in a month
```c#
//DST starts on the second Sunday of March at 2:00 AM
var dstStarts = new FloatingDate(3, 2, DayOfWeek.Sunday, 2, 0, 0);
```
### Get date & time
Getting local time
```c#
var localTime = timezone.GetLocalTime(utcDateTime);
```
Check if DST is observed at given date and time
```c#
var isDst = timezone.IsDst(utcDateTime);
```
### POSIX format
POSIX format lets you define all time zone's details in a compact string. For example "CET-1CEST,M3.5.0,M10.5.0/3" means:
- Standard time name: CET
- UTC offset: -1 hour
- DST time name: CEST
- DST starts on the last (5) Sunday (0) of March (3) at 2:00 AM (/3)
- DST ends on the last (5) Sunday (0) of October (10) at 3:00 AM (/3)

Use _TimeZoneConverter_ to parse POSIX string into _TimeZone_ object and vice versa
```c#
var timezone = TimeZoneConverter.FromPosixString(posixString);
```
