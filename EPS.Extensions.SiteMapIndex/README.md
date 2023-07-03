# EPS.Extensions.SiteMapIndex

This package was built in an attempt to overcome a specific limitation when it came to other sitemaps - the ability to have large and scalable site maps. 

Our code is somewhat simple:
- SiteMap, which is your basic site map
- SiteMapIndex, which serves as the 'parent' sitemap
- SiteMapConfig, which manages some simple settings for site map configuration
- Location, which is a location entry in the site map

## Getting Started

Let's talk sitemap compliance.

You can have 50,000 `location` entries, or you can have a max file size of 50,000,000 bytes (about ~47 MB). The SiteMap object will monitor these things for you, if you explicitly ask it to. 

You can instantiate a SiteMap in one of several different ways according to the constructors:
- `public SiteMap(Uri siteMapPath)` (no compliance checking)
- `public SiteMap(Uri siteMapPath, SiteMapConfig siteMapConfig)` (your specific compliance params)
- `public SiteMap(Uri siteMapPath, bool stayCompliant)` (automatic size limits)

A `Stack<Location>` is used by the SiteMap object for parsing. In our current library configuration, if compliance specifications are set and they are violated, *it will simply stop adding items from the stack, and it will be up to the developer to check for this and manage the stack(s) accordingly*.   

## Releases

##### 6.1.0
In this release we've introduced the `LocationBuilder` class for speeding up the process of building pages.

`SiteMap` now has a `Load` function which uses the `LocationBuilder` class to quickly add pages to the `Locations` stack.


### Roadmap
Eventually, as this library gets 'smarter', we'll have it set up so you can dump a list of page strings and it will 
self-map and everything. 