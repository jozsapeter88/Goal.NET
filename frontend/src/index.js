import React from "react";
import ReactDOM from "react-dom/client";


import { BrowserRouter } from "react-router-dom";
import App from './App';
import AppWithUserContext from "./App";


const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <AppWithUserContext/>
    </BrowserRouter>
  </React.StrictMode>
);