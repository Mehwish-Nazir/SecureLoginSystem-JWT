export interface RegisterUser{
    userID?:string | null,
    userName:string,
    email:string,
    password:string,
    confirmPassword:string,
    profileImagePath:string,
    profilePicture?: File;     // ✅ file to be uploaded

}


export interface RegistrationResponse{
    userID:string,
    username:string,
    email:string
}