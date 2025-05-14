import {Form} from "react-bootstrap";
import roomMateIcon_1 from "../../../Assets/roommateIcon_1.png";
import React, {useState} from "react";
import {toast} from "react-toastify";
import onChange = toast.onChange;

type TLocationSectionProps = {
    locationValue: string,
    setLocationValue: React.Dispatch<React.SetStateAction<string>>
}

const LocationSection = ({locationValue, setLocationValue}: TLocationSectionProps) => {

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
        setLocationValue(e.target.value);
    }

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            (e.target as HTMLInputElement).blur();
        }
    };
    return (
        <div className="row mx-5 py-5 g-0">
            <div className="col-lg-6 col-12 d-flex flex-column justify-content-end">
                <h1 className="display-0"> Let's find you a roommate!</h1>
                <h2 className="display-6 mt-3">Where do you want to rent a room?</h2>
                <div className="mt-3">
                    <i className="bi-geo-alt-fill position-absolute ms-3 mt-2 text-darkblue"></i>
                    <Form.Control className=" border-darkblue px-5 py-2" placeholder="Search locations"
                                  value={locationValue} onChange={handleInputChange} onKeyDown={handleKeyDown}
                                    cy-data="search-location"
                    />
                </div>
            </div>

            <div className="col d-inline-flex justify-content-center align-self-end">
                <img className="img-fluid d-none d-lg-block" src={roomMateIcon_1} alt=""/>
            </div>
        </div>
    );
};

export default LocationSection;
