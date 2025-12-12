import React from "react";
import Sidebar from "../Components/Sidebar";
import Navbar from "../Components/Navbar";

const Layout = ({ children }) => {
  return (
   <div className="flex h-screen">
  <aside className="fixed top-0 left-0 h-screen w-64 bg-gray-800 text-white z-50">
    <Sidebar />
  </aside>

  <div className="ml-64 w-full h-screen flex flex-col">
    <Navbar /> {/* fixed */}
<main className="flex-1 bg-gray-50 px-0 pt-12">

      {children}
    </main>
  </div>
</div>

  );
};

export default Layout;
