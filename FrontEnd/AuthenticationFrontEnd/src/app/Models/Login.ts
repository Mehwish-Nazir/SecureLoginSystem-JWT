export interface Login{
 email:string,
 password:string;
}

export interface LoginResponse{
    username:string,
    email:string,
    role:string,
    token:string | null
}