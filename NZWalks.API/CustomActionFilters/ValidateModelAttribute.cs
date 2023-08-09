using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.CustomActionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        //yeh check krega ki jo bhi hmne attribute or fields daali h dto ke upar like required field and aage bhi unsbko validate 
        //krne ke liye baar baar modelisvalid use krna pdta h controller ke ander toh wo kaafi bda ho jaata h toh isliye 
        // usko chota krne ke liye hme yeh use kra jo ki hmara kaam aasan bna dega 


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.ModelState.IsValid==false) {
                context.Result=new BadRequestResult();  
            }
        }
    }
}
