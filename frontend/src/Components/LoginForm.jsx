import { useState } from "react"
import SignIn from "../Pages/SignIn/SignIn"

const Authorize = (username, password) => {
    const loginObj = {"UserName": username, "Password": password}
    return fetch("/api/user/login", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json",
            // 'Accept': ''
        }, 
        body: JSON.stringify(loginObj)
    }).then(res => res.status)
}

const LoginForm = () => {

    const [showMsg, setShowMsg] = useState(true)

    const onSubmit = async (e) => {
        e.preventDefault();
        const username = e.target.formBasicUsername.value;
        const password = e.target.formBasicPassword.value;
        const authStatus = await Authorize(username, password);
        if(authStatus != 200) {
            setShowMsg(false);
            console.error("Wrong login credentials!")
        }   
        else if(authStatus == 200){
            console.log("Login successful!")
        }
    }
    
    return <SignIn onSubmit={onSubmit} showMsg={showMsg}></SignIn>
}

export default LoginForm