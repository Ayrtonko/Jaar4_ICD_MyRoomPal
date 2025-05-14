import AdminPanelTicketSection from "../../components/admin/AdminPanelTicketSection";
import AdminPanelUserSection from "../../components/admin/AdminPanelUserSection";

function AdminDashboard() {
     return (
       <div>
           {/*Ticket table section*/}
           <AdminPanelTicketSection/>

           {/*User search section*/}
           <AdminPanelUserSection/>
       </div>
     );
}

export default AdminDashboard;
