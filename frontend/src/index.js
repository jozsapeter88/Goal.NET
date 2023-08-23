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
import TeamManager from './Pages/TeamManager/TeamManager';
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import AddPlayerList from './Components/AddPlayerList/AddPlayerList';

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
    element: <AddPlayerList />
  }
  
]);
ReactDOM.createRoot(document.getElementById("root")).render(
  <RouterProvider router={router} />
);
reportWebVitals();
