import { useState } from "react"
import SignUp from "../Pages/SignUp/SignUp"

const Authorize = (username, password) => {
    const loginObj = {"UserName": username, "Password": password}
    return fetch("/api/user/register", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json",
            // 'Accept': ''
        }, 
        body: JSON.stringify(loginObj)
    }).then(res => res.status)
}

const RegisterForm = () => {

    const [showMsg, setShowMsg] = useState(true)

    const onSubmit = async (e) => {
        e.preventDefault();
        const username = e.target.formBasicUsername.value;
        const password = e.target.formBasicPassword.value;
        const authStatus = await Authorize(username, password);
        if(authStatus === 401) {
            setShowMsg(false);
            console.error("Username is already taken!")
        }
        else if(authStatus === 500){
            console.error("Can't communicate with server!")
        }
        else if(authStatus === 200){
            console.log("Registration successful!")
        }
    }
    
    return <SignUp onSubmit={onSubmit} showMsg={showMsg}></SignUp>
}

export default RegisterForm