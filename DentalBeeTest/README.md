# CalcAuto

The app provides a windows calculator handler, which allows you to sum 2 numbers. It is written in C# using .NET 8 WPF framework.

It is using the FlaUI library to handle the between apps communication (https://github.com/FlaUI/FlaUI)

SETUP:

Open the solution file in Visual Studio and run it with the play button. All the dependencies should be handled by visual studio.

The is an approximation of this design https://www.figma.com/design/TLXp8wAbdhiAmcNNu5sYBu/MDBEE-%5BDesktop-Task%5D?node-id=0-1&p=f&t=2QZfxh2kwfUXJAs7-0

Potential improvements:
- Process all the events as commands in the VM, and handle them in the view using bindings
- Proper localization instead of hardcoded strings
- Unit test all the interfaces
- Apply the correct fonts
- App Icon
- The UI is not pixel perfect, I would need to spend additional 2-3h to perfect the UI and since it is a test exercise I decided to focus on the functional aspect of the app.
    - Set the styles for button with colors for states and borders with corner radious
    - Set the styles for TextBoxes with states and colors
    - Apply a blurred background instead of a solid one
    - Apply the additional blurred background color on the result line
- The dark theme is not applied to the app, hence the white border