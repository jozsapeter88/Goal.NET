import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import DatePicker from './Components/DatePicker/DatePicker';
import SignIn from './Pages/SignIn/SignIn';
import Matches from './Components/Matches/Matches';
import LoginForm from './Components/LoginForm';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <LoginForm />
  </React.StrictMode>
);

reportWebVitals();
