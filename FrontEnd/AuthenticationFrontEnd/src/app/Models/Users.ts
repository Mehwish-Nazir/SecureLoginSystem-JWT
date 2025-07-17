export interface Users {
    userID: string;                   // Guid is string in TS
    username: string;                 // Matches C# property 'Username'
    email: string;
    phoneNumber: string;
    passwordHash: string;
    passwordSalt?: string | null;     // Nullable in backend, optional here
    createdAt: Date;
    isEmailConfirmed: boolean;        // Use boolean type, not literal false
    isActive: boolean;                // Use boolean type, not literal true
    lastLoginAt?: Date | null;        // Nullable datetime in backend
    failedLoginAttempts: number;
    lockOutEnd?: Date | null;         // Nullable datetime in backend
    profileImagePath?: string | null; // Nullable string in backend
    emailConfirmationToken?: string | null; // Nullable string
    emailConfirmationExpiry?: Date | null;  // Nullable datetime
}
