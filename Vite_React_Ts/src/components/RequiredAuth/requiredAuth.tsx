import React, { useContext, useEffect } from "react";
import { UserContext } from "../../context/userContext";
import { Navigate, Outlet, useLocation } from "react-router-dom";
interface RequiredAuthProps {
  allowedRoles: number [];
}

const RequiredAuth = ({ allowedRoles }: RequiredAuthProps) => {
  const { userRole } = useContext(UserContext);
  const location = useLocation();

  //   console.log("User Role: ", userRole);
  //   console.log("Condition 1: ", userRole !== undefined);
  //   console.log("Condition 2: ", userRole && allowedRoles.includes(userRole));

  if (userRole) {
    return allowedRoles.includes(userRole) ? (
      <Outlet />
    ) : userRole === 1 || userRole === 2 ? (
      <Navigate to="/unauthorized" state={{ from: location }} replace />
    ) : (
      <Navigate to="/" state={{ from: location }} replace />
    );
  } else {
    <Navigate to="/" state={{ from: location }} replace />;
  }
};

export default RequiredAuth;
