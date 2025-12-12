import React from "react";
import { NavLink } from "react-router-dom";
import {
  LayoutDashboard,
  ListCheck,
  Users,
  BarChart3,
  Settings,
  HelpCircle,
} from "lucide-react";

const Sidebar = () => {
  const menuItems = [
    { name: "Dashboard", to: "/dashboard", icon: <LayoutDashboard size={18} /> },
    { name: "Campaigns", to: "/campaigns", icon: <ListCheck size={18} /> },
    { name: "Users", to: "/users", icon: <Users size={18} /> },
    { name: "Reports", to: "/reports", icon: <BarChart3 size={18} /> },
    { name: "Settings", to: "/settings", icon: <Settings size={18} /> },
    { name: "Help", to: "/help", icon: <HelpCircle size={18} /> },
  ];

  return (
    <aside className="h-screen w-64 bg-gray-900 text-gray-200 fixed left-0 top-0 shadow-xl">
      {/* Logo / Title */}
      <div className="h-20 flex items-center justify-center border-b border-gray-700">
        <h1 className="text-xl font-semibold tracking-wide">Campaign Portal</h1>
      </div>

      {/* Menu */}
      <nav className="mt-4 space-y-2 px-4">
        {menuItems.map((item) => (
          <NavLink
            key={item.name}
            to={item.to}
            className={({ isActive }) =>
              `flex items-center gap-3 px-4 py-3 rounded-lg transition 
              ${
                isActive
                  ? "bg-indigo-600 text-white shadow-md"
                  : "hover:bg-gray-800"
              }`
            }
          >
            {item.icon}
            <span>{item.name}</span>
          </NavLink>
        ))}
      </nav>
    </aside>
  );
};

export default Sidebar;
