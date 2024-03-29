import React, { useState, useEffect } from "react";
import { Button } from "react-bootstrap";
import Autocomplete from "@mui/material/Autocomplete";
import TextField from "@mui/material/TextField";
import useCookies from "react-cookie/cjs/useCookies";
import { useNavigate } from "react-router-dom";
import auth from "../../auth/auth";

const UserEditor = () => {
  const navigate = useNavigate();
  const [cookies] = useCookies();
  const [UserLevels, setUserLevels] = useState("");
  const [Users, setUsers] = useState("");
  const [UserNames, setUserNames] = useState([]);
  const [SelectedUser, setSelectedUser] = useState("");

  if (!auth(2)) {
    window.location.href = "/home"
  }

  const [HideLevel, setHideLevel] = useState(true);
  const [HideButton, setHideButton] = useState(true);

  if (cookies["token"] === undefined || cookies["username"] === undefined) {
    navigate("/");
    console.log(cookies["token"]);
  }

  useEffect(() => {
    fetchUsers();
    fetchLevels();
  }, [SelectedUser]);

  useEffect(() => {
    if (HideLevel) setHideButton(true);
  }, [HideLevel]);

  const fetchUsers = async () => {
    try {
      const response = await fetch(process.env.REACT_APP_API_URL +`/user/getall`, {
        headers: {
          Authorization: "Bearer " + cookies["token"],
        },
      });
      if (response.ok) {
        const data = await response.json();
        setUsers(data);
        setUserNames(Object.keys(data));
      } else {
        console.error("Error fetching users:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const fetchLevels = async () => {
    try {
      const response = await fetch(process.env.REACT_APP_API_URL + "/user/levels", {
        headers: {
          Authorization: "Bearer " + cookies["token"],
        },
      });
      if (response.ok) {
        const data = await response.json();
        setUserLevels(data);
      } else {
        console.error("Error fetching user levels:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching user levels:", error);
    }
  };

  const handleChange = (e) => {
    e.target.value === undefined ? setHideLevel(true) : setHideLevel(false);
    console.log(e.target.value);
    console.log(HideLevel);
    if (e.target.value !== undefined) {
      setSelectedUser(e.target.innerText);
    }
  };
  const handleSubmit = (e) => {
    e.preventDefault();
    modifyUser(e);
    window.location.reload(false);
  };

  const modifyUser = async (e) => {
    const userData = {
      UserName: e.target["username-box"].value,
      UserLevel: UserLevels.indexOf(e.target["level-box"].value),
    };
    try {
      const response = await fetch(process.env.REACT_APP_API_URL + "/api/user/update", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + cookies["token"],
        },
        body: JSON.stringify(userData),
      });

      if (response.ok) {
        console.log("User updated:", userData);
      } else {
        console.error("Error updating user:", response.statusText);
      }
    } catch (error) {
      console.error("Error updating user:", error);
    }
  };
  return (
    <div className="container d-flex align-items-center justify-content-center vh-100">
      <div className="card">
        <div className="card-body">
          <h1 className="card-title text-center">User editor</h1>
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <Autocomplete
                disablePortal
                id="username-box"
                options={UserNames}
                onChange={handleChange}
                sx={{ width: 300 }}
                renderInput={(params) => (
                  <TextField {...params} label="Select user" />
                )}
              />
            </div>
            <div>
              <Autocomplete
                disabled={HideLevel}
                disablePortal
                id="level-box"
                options={UserLevels}
                onChange={(e) =>
                  e.target.value === undefined
                    ? setHideButton(true)
                    : setHideButton(false)
                }
                sx={{ width: 300 }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label={UserLevels[Users[SelectedUser]]}
                  />
                )}
              />
            </div>
            <div
              style={{
                display: "flex",
                justifyContent: "center",
                marginTop: "15px",
              }}
            >
              <Button
                type="submit"
                variant="outline-primary"
                size="lg"
                disabled={HideButton}
              >
                Submit
              </Button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default UserEditor;
