import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginService } from "../../Services/authService";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const authdata = await loginService(email, password);
      console.log("Logged in user", authdata);
      navigate("/campaigns");
    } catch (error) {
      setError(error.message);
    }
  };

  return (
   <div className="fixed inset-0 flex flex-col md:flex-row bg-gradient-to-br from-purple-800 via-indigo-800 to-gray-800">

  {/* Left and Right panels */}

      {/* Left Side */}
      <div className="flex-1 flex flex-col justify-center items-start px-8 md:px-12 py-6 text-gray-100">
        <h1 className="text-4xl md:text-5xl font-extrabold mb-6">
          Welcome to Campaign Management
        </h1>
        <p className="mb-4 text-lg text-gray-200">
          Manage your campaigns efficiently with our intuitive dashboard. Create, update, and track campaigns in real-time.
        </p>
        <p className="mb-4 text-gray-300">
          Designed for professionals who need clarity, control, and speed. Your campaigns, your way.
        </p>
        <ul className="list-disc ml-5 text-gray-300 space-y-2">
          <li>Easy campaign creation & management</li>
          <li>Update and delete campaigns seamlessly</li>
          <li>Professional analytics & tracking</li>
        </ul>
      </div>

      {/* Right Side */}
      <div className="flex-1 flex items-center justify-center px-6 py-8 md:p-12">
        <div className="bg-gray-900/80 backdrop-blur-sm p-8 md:p-10 rounded-3xl shadow-2xl w-full max-w-md border border-gray-700">
          <h2 className="text-3xl font-bold text-teal-300 text-center mb-4">Login</h2>
          {error && <div className="text-red-400 text-sm text-center mb-4">{error}</div>}
          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-gray-200 font-medium mb-1">Email</label>
              <input
                type="email"
                placeholder="rama@example.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full px-5 py-3 rounded-xl bg-gray-800 text-teal-100 border border-gray-600 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-400 transition"
                required
              />
            </div>
            <div>
              <label className="block text-gray-200 font-medium mb-1">Password</label>
              <input
                type="password"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full px-5 py-3 rounded-xl bg-gray-800 text-teal-100 border border-gray-600 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-400 transition"
                required
              />
            </div>
            <button
              type="submit"
              className="w-full bg-gradient-to-r from-teal-400 to-purple-400 text-gray-900 py-3 rounded-xl font-bold shadow-lg hover:scale-105 transition-transform"
            >
              Login
            </button>
          </form>
          <p className="text-center text-gray-400 mt-6 text-sm">
            Demo login: rama@example.com / password
          </p>
        </div>
      </div>
    </div>
  );
}