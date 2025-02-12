namespace SnowShovel 

module LoginTypes  = 

    type public AuthMethod = ExternalBrowser
                           | KeyPair of keyFilePath : string
                           | Password of password : string

    type public LoginDetails =
        { username: string
          account: string
          database: string
          role: string
          schema: string
          warehouse: string
          proxy: string
          auth: AuthMethod
        }

    let public emptyLoginDetails =
        { username = ""
          account = ""
          database = ""
          role = ""
          schema = ""
          warehouse = ""
          proxy = ""
          auth = ExternalBrowser
        }
