# NuGet.Services.FeatureFlags

Enables dynamic toggling of features. These features are controlled by the NuGet.org admin panel.

Examples to use this library:

```csharp
IFeatureFlagClient features = ...;

if (features.Enabled("NuGetGallery.TyposquattingDetection"))
{
    ...
}
```

```csharp
IFlightClient flights = ...;
User currentUser = ...;

if (flights.Enabled("NuGetGallery.TyposquattingDetection", currentUser))
{
    ...
}
```