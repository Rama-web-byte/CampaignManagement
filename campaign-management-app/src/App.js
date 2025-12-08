import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import CreateCampaign from '../src/Components/AddCampaign'; // Import the CreateCampaign component
import CampaignList from '../src/Components/CampaignList'; // Import the CampaignList component
import WorkOut from '../src/Components/workout'; // Import the CampaignList component
import LoginPage from './features/auth/LoginPage';

const App = () => {
  return (
    <Router>
      <div className="p-6">
        <header className="mb-6">
        <h1 className="text-3xl font-bold">Campaign Management</h1>
        <nav className="mt-2">
          <ul className="flex gap-4 list-none p-0">
            <li>
              <Link className="text-blue-600 hover:underline" to="/list">Campaign List</Link>
              </li>
            
            <li>
              <Link className="text-blue-600 hover:underline" to="/create-campaign">
              Create Campaign
              </Link>
              </li>
              <li>
              <Link className="text-blue-600 hover:underline" to="/workout">
              WorkOut
              </Link>
            </li>
            <li>
              <Link className="text-blue-600 hover:underlin" to="/login">
              Login
              </Link>
            </li>
          </ul>
        </nav>
        </header>
        <Routes>
          <Route path="/" element={<WorkOut/>} /> {/* Home Page */}
          <Route path="/list" element={<CampaignList/>} /> {/* Home Page */}
          <Route path="/create-campaign" element={<CreateCampaign />} /> {/* Create Campaign Page */}
          <Route path="/login" element={<LoginPage/>}/>
        </Routes>
      </div>
    </Router>
  );
};

export default App;
