import React from "react";
import { createHashRouter, useNavigate } from "react-router-dom";
import { logoutService } from "../Services/authService";

const Navbar = () => {
  const navigate = useNavigate();

  const handleLogout=async()=>
  {
    try
    {
      await logoutService();
      navigate("/login");

    }
     catch(error)
     {
      console.log("Logout failed");
     }

  };

  return (
<nav className="fixed top-0 left-64 right-4 h-20 bg-gray-900 text-gray-200 z-40 flex items-center justify-between px-6">



  {/* Left side: Title */}
  <h1 className="text-xl font-bold tracking-wide"></h1>

  {/* Right side: Actions */}
   <div className="flex items-center space-x-6">
    <button onClick={() => navigate("/profile")} className="hover:text-gray-300">
      MyProfile
    </button>
    <button onClick={handleLogout} className="hover:text-gray-300">
      Logout
    </button>
  </div>

</nav>
  );
};

export default Navbar;
