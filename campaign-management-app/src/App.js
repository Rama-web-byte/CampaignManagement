import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import CreateCampaign from '../src/Components/AddCampaign'; // Import the CreateCampaign component
import CampaignList from '../src/Components/CampaignList'; // Import the CampaignList component
import WorkOut from '../src/Components/workout'; // Import the CampaignList component

const App = () => {
  return (
    <Router>
      <div style={{ padding: '20px' }}>
        <h1>Campaign Management</h1>
        <nav>
          <ul style={{ listStyleType: 'none', padding: 0 }}>
            <li style={{ display: 'inline', marginRight: '20px' }}>
              <Link to="/">Campaign List</Link>
            </li>
            <li style={{ display: 'inline' }}>
              <Link to="/create-campaign">Create Campaign</Link>
            </li>
          </ul>
        </nav>
        <Routes>
          <Route path="/" element={<WorkOut/>} /> {/* Home Page */}
          <Route path="/list" element={<CampaignList/>} /> {/* Home Page */}
          <Route path="/create-campaign" element={<CreateCampaign />} /> {/* Create Campaign Page */}
        </Routes>
      </div>
    </Router>
  );
};

export default App;
