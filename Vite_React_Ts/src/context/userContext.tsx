import { ReactNode, createContext, useEffect, useState } from "react";

interface UserProviderProps {
  children: ReactNode;
}

interface UserContextType {
  role: string | undefined;
  login: (role: string, token: string) => void;
  logout: () => void;
}

export const UserContext = createContext<UserContextType>({
  role: undefined,
  login: () => {},
  logout: () => {},
});

const UserProvider = ({ children }: UserProviderProps) => {
  const [role, setRole] = useState<string | undefined>(undefined);

  useEffect(() => {
    const storageToken = localStorage.getItem("token");
    const storageRole = localStorage.getItem("role");

    if (storageToken && storageRole) {
      setRole(storageRole);
    }
  }, []);

  const login = (role: string, token: string) => {
    setRole(role);

    localStorage.setItem("role", role);
    localStorage.setItem("token", token);
  };

  const logout = () => {
    setRole(undefined);

    localStorage.removeItem("role");
    localStorage.removeItem("token");
  };

  return (
    <UserContext.Provider value={{ role, login, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;
