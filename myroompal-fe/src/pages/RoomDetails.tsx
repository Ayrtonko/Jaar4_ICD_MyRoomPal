import React, {useEffect, useState} from 'react';
import { useParams } from 'react-router-dom';
import {ApartmentData, ApartmentDetailData, ApartmentService} from "../services/ApartmentService";
import EnumSpinnerSize from "../models/EnumSpinnerSize";
import Spinner from "../components/Spinner";

const RoomDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [apartment, setApartment] = useState<ApartmentDetailData>()
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchApartment = async () => {
            if (id) {
                try {
                    const data = await ApartmentService.getApartmentById(id);
                    setApartment(data);
                } catch (error) {
                    console.error("Error fetching apartment data:", error);
                } finally {
                    setLoading(false);
                }
            } else {
                console.error("ID is undefined");
                setLoading(false);
            }
        };

        setLoading(true);
        fetchApartment();
    }, [id]);


    return (
        <div className="container mt-4">
            {loading ?
                <div className="d-flex justify-content-center mt-5">
                    <Spinner size={EnumSpinnerSize.XL}/>
                </div>
                :
                <div>
                    <h1>{apartment?.roomName}</h1>
                    <h2>{apartment?.address.streetName}, {apartment?.address.postalCode}, {apartment?.address.city}, {apartment?.address.country}</h2>
                    <div className="d-flex flex-column justify-content-between align-items-center my-4">
                        <img
                            src={apartment?.imageLink}
                            className="card-img-top"
                            style={{height: "400px", width: "100%", objectFit: "cover"}}
                        />
                        <div className="d-flex justify-content-between w-100 mt-4">
                            <div className="w-50">
                                <h3>Details</h3>
                                <p>{apartment?.description}</p>
                            </div>
                            <div className="w-25">
                                <h3>Pricing</h3>
                                <p>{apartment?.rentPrice} € / month</p>
                            </div>
                            <div className="w-25">
                                <h3>Size</h3>
                                <p>{apartment?.size} m²</p>
                            </div>
                        </div>
                    </div>
                </div>
            }
        </div>
    );
};

export default RoomDetails;
