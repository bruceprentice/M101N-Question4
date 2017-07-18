# M101N-Question4
Question 4 final Exam M101N, crafted as aspnet core application with MongoDb identity

## This is my attempt to work Question 4 of the Mongo DB for .Net Developers Final Exam.

A few things to note:  
 -The application works just fine, but to work with mongoProc, you will notice that I modified the following:
 - Added "Name" In ApplicationUser class, then exposed it as a claim that I could get to.  The original solution used two properties 
  "Name" and "Email".  I used the [Microsoft.AspNetCore.Identity.MongoDB][AINuget] nuget package. 
  - In LoginViewModel.cs, I hard coded the password as default 
 ```c#
  public string Password { get; set; } = "test123";
  ```
  -  Note that I also added a button for Animal test from Question 8, so I could test what the answer would be. 
  
  ###TODO
  If you want a working model that passes mongoProc perfectly then take their implementation that uses only Name and Email and implement 
  that as a hacked version of Identity. 
  
  
  
  [AINuget]: <https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.MongoDB/>
