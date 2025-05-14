import TicketTable from "./TicketTable";

function AdminPanelTicketSection (){
    return (
        <div className="page-container">
            <h2 className="mt-5 fs-1 fw-semibold">Administration panel</h2>
            <TicketTable/>
        </div>
    )
}

export default AdminPanelTicketSection;
