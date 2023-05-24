import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import DatePicker from './Components/DatePicker/DatePicker';
import SignIn from './Pages/SignIn/SignIn';
import Matches from './Components/Matches/Matches';
import LoginForm from './Components/LoginForm';
import RegisterForm from './Components/RegisterForm';

import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
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
    path: "/register",
    element: <RegisterForm />,
  },
]);
ReactDOM.createRoot(document.getElementById("root")).render(
  <RouterProvider router={router} />
);
reportWebVitals();
