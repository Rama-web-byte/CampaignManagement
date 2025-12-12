import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link ,Navigate} from 'react-router-dom';
import CreateCampaign from '../src/features/campaign/AddCampaign'; // Import the CreateCampaign component
import CampaignList from '../src/features/campaign/CampaignList'; // Import the CampaignList component
import WorkOut from '../src/features/campaign/Workout'; // Import the CampaignList component
import LoginPage from './features/auth/Login';
import ViewCampaign from './features/campaign/ViewCampaign';
import EditCampaign from './features/campaign/UpdateCampaign';
import ProtectedRoute from './features/auth/ProtectedRoute';

const App = () => {
  return (
    <Router>
      <div className="p-6">
        <Routes>
          <Route path="/" element={<Navigate to="/login" replace/>}/>
          <Route path="/login" element={<LoginPage/>}/>
          <Route path="/workout" element={<ProtectedRoute><WorkOut/></ProtectedRoute>} /> 
          <Route path="/campaigns" element={<ProtectedRoute><CampaignList/></ProtectedRoute>} /> 
          <Route path="/create-campaign" element={<ProtectedRoute><CreateCampaign /></ProtectedRoute>} /> 
          <Route path="/campaign/view/:id" element={<ProtectedRoute><ViewCampaign /></ProtectedRoute>}/>
          <Route path="/campaign/edit/:id" element={<ProtectedRoute><EditCampaign/></ProtectedRoute>}></Route>
        </Routes>
      </div>
    </Router>
  );
};

export default App;
