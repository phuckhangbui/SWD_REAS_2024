import { fromAddress, setKey, geocode, RequestType } from "react-geocode";

export const handleGeoCode = async (address: string) => {
  try {
    const response = await geocode(RequestType.ADDRESS, address)
    const { results } = response;
    const { lat, lng } = results[0].geometry.location;
    return { lat, lng };
  } catch (error) {
    console.log("Error geocoding: " + error);
  }
};
