# SOLAMI-UnityClient

## Project Introduction
This project is designed to work in conjunction with the SOLAMI project, providing VR visualization of SOLAMI algorithm results. SOLAMI project can be found here: [SOLAMI GitHub Repository](https://github.com/AlanJiang98/SOLAMI)

## Installation Guide
This project is developed using Unity 2022.3.59f1. The project requires several Unity Packages and Nuget Packages. Please install them in the following order:

1. [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)
2. [Meta XR All-in-One SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
3. [Dynamic Bone](https://assetstore.unity.com/packages/tools/animation/dynamic-bone-16743)
4. [Colourful Hierarchy Category GameObject](https://assetstore.unity.com/packages/tools/utilities/colourful-hierarchy-category-gameobject-205934)
5. StackExchange.Redis 2.1.58 (NuGet Package)
6. WebSocketSharp-netstandard 1.0.1 (NuGet Package)

## Intellectual Property Statement
The 3D assets used in this project are sourced from Sketchfab and Vroid. These assets have been modified to fit the project's needs. Please note that these assets should not be used for commercial purposes. The original assets can be found at:

- [Late #cuterobotchallenge](https://sketchfab.com/3d-models/late-cuterobotchallenge-cb2a7911a5f243dcbe8480946a3bd5fe)
- [Batman Ben Affleck from The Flash 2023](https://sketchfab.com/3d-models/batman-ben-affleck-from-the-flash-2023-33ca17095d3148218958b8f39c8efe64)
- [Animated Donald Trump 3D Cartoon Caricature](https://sketchfab.com/3d-models/animated-donald-trump-3d-cartoon-caricature-1082dd6a29624968a927c806793aacfe)
- [Banana Cat](https://sketchfab.com/3d-models/banana-cat-a738b17630854e9894505b139601d75d)
- [Ocarina of Time Link](https://sketchfab.com/3d-models/ocarina-of-time-link-c62717add333410987482d44959e56c7)
- [Chappie Blender Rig Free](https://sketchfab.com/3d-models/chappie-blender-rig-free-fc2424ff8ab840ac907d38dc073d1327)

## License and Usage Notices
In this repository, we provide code for raw data preprocessing, multimodal data synthesis, SOLAMI model training, model evaluation, VR Unity client and server code for community reference. Considering that we used some company internal data to train the models in the original paper, we are not open-sourcing the raw data and trained models. Users can use their own collected data to train their deployable models on advanced end-to-end multimodal models.

Usage and License Notices: This project utilizes certain datasets, 3D assets, and checkpoints that are subject to their respective original licenses. Users must comply with all terms and conditions of these original licenses, including but not limited to the OpenAI Terms of Use for generating synthetic data scripts, Llama community license for foundation language models, SMPL-X for original motion format, and HumanML3D, Inter-X, DLP-MoCap, Anyinstruct, commonVoice for data generation and model training. This project does not impose any additional constraints beyond those stipulated in the original licenses. Furthermore, users are reminded to ensure that their use of the dataset and checkpoints is in compliance with all applicable laws and regulations.