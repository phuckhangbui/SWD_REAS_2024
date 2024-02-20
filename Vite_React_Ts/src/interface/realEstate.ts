interface realEstate {
  id: number;
  name: string;
  address: string;
  image: string;
  price: string;
  description: string;
  status: number;
  dateStart: Date;
  dateEnd: Date;
  message: string;
  ownerId: number;
  dateCreated: Date;
}

export default realEstate;
