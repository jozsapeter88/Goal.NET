import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import UserEditor from './Components/AdminTools/UserEditor';
import HomePage from './Pages/Home/HomePage';
import LoginForm from './Components/LoginForm';
import RegisterForm from './Components/RegisterForm';
import PlayerCreator from './Components/PlayerCreator/PlayerCreator';
import TeamCreator from './Components/TeamCreator/TeamCreator';
import PlayerList from './Components/PlayerList/PlayerList';
import TeamManager from './Components/TeamManager/TeamManager'
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import TeamList from './Components/TeamList/TeamList';

const root = ReactDOM.createRoot(document.getElementById('root'));

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
  },
  {
    path: "/userEditor",
    element: <UserEditor />
  },
  {
    path: "/teamManager",
    element: <TeamManager />
  },
  {
    path: "/teamList",
    element: <TeamList />
  }
  
]);
ReactDOM.createRoot(document.getElementById("root")).render(
  <RouterProvider router={router} />
);
reportWebVitals();
