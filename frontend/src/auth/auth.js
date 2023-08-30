import { min } from "moment/moment";
import React, { useState, useEffect } from "react";
import useCookies from "react-cookie/cjs/useCookies";

const auth = (minimumLevel) => {
    const [cookies, setCookie, removeCookie] = useCookies();
    const levels = cookies["userlevels"];
    console.log(levels)
    const isLevelANumber = Number.isInteger(minimumLevel);
    if (!isLevelANumber && levels.includes(minimumLevel)) minimumLevel = levels.indexOf(minimumLevel)
    else if (!isLevelANumber){
        console.error("Error: provided minimum level is not a valid parameter");
        return false;
    }
    if (!levels.includes(cookies["userlevel"])) return false;
    else return levels.indexOf(cookies["userlevel"]) >= minimumLevel
}

export default auth;