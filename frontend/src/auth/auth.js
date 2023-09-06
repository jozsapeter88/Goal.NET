import { min } from "moment/moment";
import React, { useState, useEffect } from "react";
import useCookies from "react-cookie/cjs/useCookies";

const auth = (minimumLevel) => {
    const [cookies, setCookie, removeCookie] = useCookies();
    const levels = cookies["userlevels"];
    const level = capitalize(cookies["userlevel"].toLowerCase());
    const isLevelANumber = Number.isInteger(minimumLevel);
    if (!isLevelANumber && levels.includes(minimumLevel)) minimumLevel = levels.indexOf(minimumLevel)
    else if (!isLevelANumber){
        console.error("Error: provided minimum level is not a valid parameter");
        return false;
    }
    if (!levels.includes(level)) return false;
    else return levels.indexOf(level) >= minimumLevel
}

function capitalize(level) {
    return level[0].toUpperCase() + level.substring(1)
}

export default auth;