# IMCTaxCalcService

Thanks kindly for taking the time to review my solution, I appreciate it.

I normally consult with my product owner and architectur about some decisions I made, for example which inputs that TaxJar supports should be exposed on my service contract.  If there are concepts TaxJar supports which aren't supported on other calculators, we may consider dropping those values entirely if there's not a strong business need.  This is much easier to do if you know all APIs you intend to integrate with up front.

## Notes:
* To test, launch the solution and use the swagger page
* Service is OpenAPI/Swagger enabled
* Service supports versioning
* Uncaught exception handling via MS logging framework
* Decorator pattern for tax client tracing, factory pattern for client construction based on user, Dependency injection via MS DI (have used both MS' framework and ninject on .net framework projects)
* Separations between layers of app down to DTOs/app objects (tax calc service DTOs, TaxClient contract app objects, TaxJar integration DTOs)
