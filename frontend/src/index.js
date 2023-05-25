import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import DatePicker from './Components/DatePicker/DatePicker';
import SignIn from './Pages/SignIn/SignIn';
import Matches from './Components/Matches/Matches';
import HomePage from './Pages/Home/HomePage';
import LoginForm from './Components/LoginForm';
import RegisterForm from './Components/RegisterForm';
import PlayerCreator from './Components/PlayerCreator/PlayerCreator';
import TeamCreator from './Components/TeamCreator/TeamCreator';
import PlayerList from './Components/PlayerList/PlayerList';
import { createBrowserRouter, RouterProvider } from "react-router-dom";
// import Root, { rootLoader } from "./routes/root";

const root = ReactDOM.createRoot(document.getElementById('root'));
// root.render(
//   <React.StrictMode>
//     <LoginForm />
//   </React.StrictMode>
// );

const router = createBrowserRouter([
  {
    path: "/",
    element: <LoginForm />,
  },
  {
    path: "/home",
    element: <HomePage />,
  },
  {
    path: "/register",
    element: <RegisterForm />,
  },
  {
    path: "/createPlayer",
    element: <PlayerCreator />,
  },
  {
    path: "/createTeam",
    element: <TeamCreator />,
  },
  {
    path: "/playerList",
    element: <PlayerList />
  }
  
]);
ReactDOM.createRoot(document.getElementById("root")).render(
  <RouterProvider router={router} />
);
reportWebVitals();
