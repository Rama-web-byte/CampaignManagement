const API_URL = "/api/User/login"; 
console.log(API_URL);

export async function loginService(email,password)
{
    try{
    const response=await fetch(API_URL,
        {
            method:"POST",
            headers:
            {
                "Content-Type":"application/json",
            },
            body:JSON.stringify({Email:email,Password:password}),
        });

        if(!response.ok)
        {
            const err=await response.json();
            throw new Error(err.message || "Login failed");
            
        }

        const data=await response.json();
        localStorage.setItem("token",data.token);
        console.log("Token:",localStorage.getItem("token"));
        localStorage.setItem("username",data.userName);
        localStorage.setItem("role",data.role);
      return data;
    }
 catch(error)
 {
    throw error;
 }
};

export const logoutService=()=>
{
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    localStorage.removeItem("role");
}
        