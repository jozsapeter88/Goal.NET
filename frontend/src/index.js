import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import DatePicker from './Components/DatePicker/DatePicker';
import SignIn from './Components/SignIn/SignIn';
import Matches from './Components/Matches/Matches';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <SignIn />
  </React.StrictMode>
);

reportWebVitals();
