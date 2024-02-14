import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PageNotFound from "./Pages/PageNotFound";
import { AdminLayout } from "./Pages/Admin/AdminLayout";
import AdminDashboard from "./Pages/Admin/AdminDashboard";
import AuctionOngoing from "./Pages/Admin/AdminAuctionOngoing";
import StaffList from "./Pages/Admin/StaffList";
import MemberList from "./Pages/Admin/MemberList";
import AuctionComplete from "./Pages/Admin/AdminAuctionComplete";
import AuctionDetail from "./Pages/Admin/AdminAuctionDetail";

function App() {
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<AdminLayout />} />
          <Route path="/admin" element={<AdminLayout />}>
            <Route index element={<AdminDashboard />} />
            <Route path="auction/ongoing" element={<AuctionOngoing />} />
            <Route path="auction/complete" element={<AuctionComplete />} />
            <Route path="auction/detail/:key" element={<AuctionDetail />} />
            <Route path="user/staff" element={<StaffList />} />
            <Route path="user/member" element={<MemberList />} />
            <Route path="*" element={<PageNotFound />} />
          </Route>
          <Route></Route>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
